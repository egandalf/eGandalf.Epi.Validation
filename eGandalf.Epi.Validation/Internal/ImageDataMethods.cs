using EPiServer.Core;

namespace eGandalf.Epi.Validation.Internal
{
    internal class ImageDataMethods
    {
        /// <summary>
        /// Uses System.Drawing to generate an in-memory Image object from Episerver ImageData.
        /// </summary>
        /// <param name="imageData">An instance of an Episerver-managed image asset.</param>
        /// <returns>ImageData.BinaryData stream as an in-memory System.Drawing.Image</returns>
        internal static System.Drawing.Image ToSystemImage(ImageData imageData)
        {
            try
            {
                using (var stream = imageData.BinaryData.OpenRead())
                {
                    using (var image = System.Drawing.Image.FromStream(stream))
                    {
                        return image;
                    }
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
