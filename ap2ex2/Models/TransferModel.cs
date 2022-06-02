using System.ComponentModel.DataAnnotations;

namespace ap2ex2.Models
{
    public class TransferModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Content { get; set; }
    }
}