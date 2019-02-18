namespace MonkeyButler.Lodestone.Services.Web
{
    internal class HttpResponse : ResponseBase
    {
        public string Body { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
