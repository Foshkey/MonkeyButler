namespace MonkeyButler.Lodestone.Web {
    internal class HttpResponse : ResponseBase {
        public string Body { get; set; }
        public bool IsSuccessful { get; set; }
    }
}
