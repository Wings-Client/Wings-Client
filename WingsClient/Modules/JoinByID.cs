public void JoinWorldByID()
{
    if (bUseClipboard)
        Helper.JoinWorldById(System.Windows.Forms.Clipboard.GetText().Trim());
    else
        KiraiLib.HUDInput("World ID", "Join", "wrld_*:?????~*()~nonce()", new Action<string>((resp) =>
        {
            Helper.JoinWorldById(resp.Trim());
        }));
}

public void CopyWorldID()
{
    string id = $"{RoomManager.field_Internal_Static_ApiWorld_0.id}:{RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags}";
    System.Windows.Forms.Clipboard.SetText(id);
    MelonLoader.MelonLogger.Msg(id);
    KiraiLib.Logger.Display("World ID copied to clipboard and saved to console", 2);
}