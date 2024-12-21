namespace SignalRWebUI.Dtos.MailDtos
{
    public class CreateMailDto
    {
        // alıcının mail adresi
        public string ReceiverMail { get; set; }
        public string Subject { get; set; }

        // MEsaj
        public string Body { get; set; }
    }
}
