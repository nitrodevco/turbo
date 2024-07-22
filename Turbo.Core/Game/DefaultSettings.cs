using System.Text.RegularExpressions;
using Turbo.Core.Game.Rooms.PathFinder.Constants;

namespace Turbo.Core.Game;

public class DefaultSettings
{
    public static int GameCycleMs = 500;
    public static int StoreCycleMs = 2000;
    public static HeuristicFormula PathingFormula = HeuristicFormula.ReverseDijkstra;
    public static bool PathingAllowsDiagonals = true;
    public static double MaximumStepHeight = 2.0;
    public static double MaximumFurnitureHeight = 40.0;
    public static int TileHeightDefault = 32767;
    public static int TileHeightMultiplier = 256;
    public static double MinimumStackHeight = 0.001;
    public static int FurniPerFragment = 100;
    public static int BadgesPerFragment = 100;
    public static int MaxActiveBadges = 5;
    public static int DiceCycles = 4;
    public static string Delimiter = ";";
    public static int MaximumChatDistance = 50;
    public static Regex RoomNameRegex = new("^(?=.*[A-Z,a-z,0-9]).{3,60}$");
    public static Regex RoomDescriptionRegex = new("^(?=.*[A-Z,a-z,0-9]).{1,60}$");
    public static Regex RoomPasswordRegex = new("^(?=.*[A-Z,a-z,0-9]).{1,60}$");
    public static int MaximumUsersPerRoom = 100;
    public static int RoomDisposeTicks = 120;
    public static int RoomTryDisposeTicks = 1200;
    public static int HeadTurnCycles = 6;
    public static int AvatarIdleCycles = 1200;
    public static int RollerCycles = 4;
    public static int MaxPlayersPerTeam = 5;
    public static string DefaultFloorLogicName = "default_floor";
    public static string DefaultWallLogicName = "default_wall";
}