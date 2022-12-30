namespace DesktopFox 
{ 
    public enum Animation
    {
        /// <summary>
        /// Es wird keine Animation ausgeführt
        /// </summary>
        None = 0,

        /// <summary>
        /// Fenster erscheint
        /// </summary>
        PopInAnimation = 1,

        /// <summary>
        /// Fenster verschwindet
        /// </summary>
        PopOutAnimation = 2,

        /// <summary>
        /// Fenster Deckkraft wird langsam erhöht
        /// </summary>
        FadeInAnimation = 3,

        /// <summary>
        /// FensterDeckkraft wird langsam verringert
        /// </summary>
        FadeOutAnimation = 4,
    }
}
