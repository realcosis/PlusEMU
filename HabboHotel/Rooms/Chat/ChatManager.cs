using NLog;
using Plus.HabboHotel.Rooms.Chat.Commands;
using Plus.HabboHotel.Rooms.Chat.Emotions;
using Plus.HabboHotel.Rooms.Chat.Filter;
using Plus.HabboHotel.Rooms.Chat.Logs;
using Plus.HabboHotel.Rooms.Chat.Pets.Commands;
using Plus.HabboHotel.Rooms.Chat.Pets.Locale;
using Plus.HabboHotel.Rooms.Chat.Styles;

namespace Plus.HabboHotel.Rooms.Chat;

public sealed class ChatManager
{
    private static readonly ILogger Log = LogManager.GetLogger("Plus.HabboHotel.Rooms.Chat.ChatManager");

    /// <summary>
    /// Chat styles.
    /// </summary>
    private readonly ChatStyleManager _chatStyles;

    /// <summary>
    /// Commands.
    /// </summary>
    private readonly CommandManager _commands;

    /// <summary>
    /// Chat Emoticons.
    /// </summary>
    private readonly ChatEmotionsManager _emotions;

    /// <summary>
    /// Filter Manager.
    /// </summary>
    private readonly WordFilterManager _filter;

    /// <summary>
    /// Chatlog Manager
    /// </summary>
    private readonly ChatlogManager _logs;

    /// <summary>
    /// Pet Commands.
    /// </summary>
    private readonly PetCommandManager _petCommands;

    /// <summary>
    /// Pet Locale.
    /// </summary>
    private readonly PetLocale _petLocale;

    /// <summary>
    /// Initializes a new instance of the ChatManager class.
    /// </summary>
    public ChatManager()
    {
        _emotions = new ChatEmotionsManager();
        _logs = new ChatlogManager();
        _filter = new WordFilterManager();
        _filter.Init();
        _commands = new CommandManager(":");
        _petCommands = new PetCommandManager();
        _petLocale = new PetLocale();
        _chatStyles = new ChatStyleManager();
        _chatStyles.Init();
        Log.Info("Chat Manager -> LOADED");
    }

    public ChatEmotionsManager GetEmotions() => _emotions;

    public ChatlogManager GetLogs() => _logs;

    public WordFilterManager GetFilter() => _filter;

    public CommandManager GetCommands() => _commands;

    public PetCommandManager GetPetCommands() => _petCommands;

    public PetLocale GetPetLocale() => _petLocale;

    public ChatStyleManager GetChatStyles() => _chatStyles;
}