namespace OBSAudioMixer
{
    class AuthClass
    {
        public string IP;
        public string Port;
        public string Password;

        public AuthClass(string ip, string port, string password)
        {
            IP = ip;
            Port = port;
            Password = password;
        }
    }
}
