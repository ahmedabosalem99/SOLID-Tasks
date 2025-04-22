using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Day1
{
    public interface IEmailValidator
    {
        bool Validate(string email);
    }

    public interface IEmailSender
    {
        bool Send(MailMessage message);
    }

 
    public class EmailValidator : IEmailValidator
    {
        public bool Validate(string email)
        {
            return email.Contains("@");
        }
    }

    public class SmtpEmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;

        public SmtpEmailSender(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;
        }

        public bool Send(MailMessage message)
        {
            return _smtpClient.Send(message);
        }
    }

    public class UserService
    {
        private IEmailValidator _emailValidator;
        private IEmailSender _emailSender;

        public UserService(
            IEmailValidator emailValidator, IEmailSender emailSender)
        {
            _emailValidator = emailValidator;
            _emailSender = emailSender;
        }

        public void Register(string email, string password)
        {
            if (!_emailValidator.Validate(email))
                throw new ValidationException("Email is not valid");

            var user = new User(email, password);

            var message = new MailMessage("mysite@nowhere.com", email)
            {
                Subject = "Welcome to our site"
            };

            _emailSender.Send(message);
        }
    }
}
