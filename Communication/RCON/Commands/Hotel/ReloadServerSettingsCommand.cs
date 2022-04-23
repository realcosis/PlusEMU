namespace Plus.Communication.Rcon.Commands.Hotel;

internal class ReloadServerSettingsCommand : IRconCommand
{
    public string Description => "This command is used to reload the server settings.";

    public string Key => "reload_server_settings";
    public string Parameters => "";

    public bool TryExecute(string[] parameters)
    {
        PlusEnvironment.GetSettingsManager().Reload();
        return true;
    }
}