namespace PizzaCase.Utils
{
    public sealed class Config
    {
        private static readonly Config instance = new Config();

        private Config()
        {
            // Initialiseer standaardwaarden of laad uit een configuratiebestand
            CommunicationProtocol = "TCP"; // Default protocol
        }

        public static Config Instance
        {
            get { return instance; }
        }

        public string CommunicationProtocol { get; set; }
    }
}
