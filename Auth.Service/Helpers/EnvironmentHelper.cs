namespace Auth.Service.Helpers;

public class EnvironmentHelper
{
    public static string WebRootPath { get; set; }
    public static string AttachmentPath => Path.Combine(WebRootPath, "Attachment");
    public static string DocFilePath => Path.Combine(WebRootPath, "DocFile");
    public static string FilePath => Path.Combine(WebRootPath, "Images");
}
