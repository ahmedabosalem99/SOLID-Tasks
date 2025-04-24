using System;
using System.Collections.Generic;
using System.Text;
using DryIoc;
using Ninject;

namespace UnifiedContainerApp
{
    public interface IOperationHandler
    {
        void Execute();
    }


    public class Document
    {
        public string Path { get; set; }
        public virtual string LoadContent()
        {
            // Simulate loading content from file
            return $"Content from {Path}";
        }
        public virtual void SaveContent()
        {
            // Simulate saving content to file
            Console.WriteLine($"Saving to {Path}");
        }
    }

    public class ReadOnlyDocument : Document
    {
        public override string LoadContent()
        {
            return $"[ReadOnly] Content from {Path}";
        }
        public override void SaveContent()
        {
            // Skips saving and logs
            Console.WriteLine($"Cannot save {Path}: File is read-only.");
        }
    }

    public class DocGroupManager
    {
        public List<Document> Documents { get; set; }

        public string AggregateContents()
        {
            var sb = new StringBuilder();
            foreach (var doc in Documents)
            {
                sb.AppendLine(doc.LoadContent());
            }
            return sb.ToString();
        }

        public void SaveAll()
        {
            foreach (var doc in Documents)
            {
                doc.SaveContent();
            }
        }
    }

    
    public class OperationHandler : IOperationHandler
    {
        private readonly DocGroupManager docGroupMgr;
        public OperationHandler(DocGroupManager docGroupMgr)
        {
            this.docGroupMgr = docGroupMgr;
        }

        public void Execute()
        {
            Console.WriteLine("Handler started.");
            // Simulate loading documents
            List<Document> docs = new List<Document>
            {
                new ReadOnlyDocument { Path = "alpha.sql" },
                new ReadOnlyDocument { Path = "beta.sql" }
            };
            docGroupMgr.Documents = docs;

            // Aggregate and show file contents
            string combined = docGroupMgr.AggregateContents();
            Console.WriteLine(combined);

            // Try saving
            docGroupMgr.SaveAll();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var container = new DryIoc.Container();

            container.Register<IOperationHandler, OperationHandler>();
            container.Register<OperationHandler>();
            container.Register<DocGroupManager>();

            IKernel kernel = new StandardKernel();
            kernel.Bind<DocGroupManager>().ToSelf();
            kernel.Bind<IOperationHandler>().To<OperationHandler>();

            var handler1 = container.Resolve<IOperationHandler>();
            var handler2 = kernel.Get<IOperationHandler>();

            Console.WriteLine("Output from DryIoc:");
            handler1.Execute();

            Console.WriteLine("Output from Ninject:");
            handler2.Execute();
        }
    }
}
