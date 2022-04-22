using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Plus.Core;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Rooms.Games.Teams;
using System.Collections.Concurrent;
using Plus.HabboHotel.Rooms.PathFinding;

namespace Plus.HabboHotel.Rooms
{
    public class Gamemap
    {
        private Room _room;
        private byte[,] _gameMap; // 0 = none, 1 = pool, 2 = normal skates, 3 = ice skates

        public bool DiagonalEnabled { get; set; }
        private RoomModel _model;
        private byte[,] _userItemEffect;
        private double[,] _itemHeightMap;
        private DynamicRoomModel _dynamicModel;
        private ConcurrentDictionary<Point, List<int>> _coordinatedItems;
        private ConcurrentDictionary<Point, List<RoomUser>> _userMap;

        public Gamemap(Room room, RoomModel model)
        {
            _room = room;
            _model = model;
            DiagonalEnabled = true;

            _dynamicModel = new DynamicRoomModel(_model);
            _coordinatedItems = new ConcurrentDictionary<Point, List<int>>();
            _itemHeightMap = new double[Model.MapSizeX, Model.MapSizeY];
            _userMap = new ConcurrentDictionary<Point, List<RoomUser>>();
        }

        public void AddUserToMap(RoomUser user, Point coord)
        {
            if (_userMap.ContainsKey(coord))
            {
                _userMap[coord].Add(user);
            }
            else
            {
                var users = new List<RoomUser>
                {
                    user
                };
                _userMap.TryAdd(coord, users);
            }
        }

        public void TeleportToItem(RoomUser user, Item item)
        {
            if (item == null || user == null)
                return;

            GameMap[user.X, user.Y] = user.SqState;
            UpdateUserMovement(new Point(user.Coordinate.X, user.Coordinate.Y), new Point(item.Coordinate.X, item.Coordinate.Y), user);
            user.X = item.GetX;
            user.Y = item.GetY;
            user.Z = item.GetZ;

            user.SqState = GameMap[item.GetX, item.GetY];
            GameMap[user.X, user.Y] = 1;
            user.RotBody = item.Rotation;
            user.RotHead = item.Rotation;

            user.GoalX = user.X;
            user.GoalY = user.Y;
            user.SetStep = false;
            user.IsWalking = false;
            user.UpdateNeeded = true;
        }

        public void UpdateUserMovement(Point oldCoord, Point newCoord, RoomUser user)
        {
            RemoveUserFromMap(user, oldCoord);
            AddUserToMap(user, newCoord);
        }

        public void RemoveUserFromMap(RoomUser user, Point coord)
        {
            if (_userMap.ContainsKey(coord))
                ((List<RoomUser>)_userMap[coord]).RemoveAll(x => x != null && x.VirtualId == user.VirtualId);
        }

        public bool MapGotUser(Point coord)
        {
            return (GetRoomUsers(coord).Count > 0);
        }

        public List<RoomUser> GetRoomUsers(Point coord)
        {
            if (_userMap.ContainsKey(coord))
                return (List<RoomUser>)_userMap[coord];
            else
                return new List<RoomUser>();
        }

        public Point GetRandomWalkableSquare()
        {
            var walkableSquares = new List<Point>();
            for (var y = 0; y < _gameMap.GetUpperBound(1); y++)
            {
                for (var x = 0; x < _gameMap.GetUpperBound(0); x++)
                {
                    if (_model.DoorX != x && _model.DoorY != y && _gameMap[x, y] == 1)
                        walkableSquares.Add(new Point(x, y));
                }
            }

            var randomNumber = PlusEnvironment.GetRandomNumber(0, walkableSquares.Count);
            var i = 0;

            foreach (var coord in walkableSquares.ToList())
            {
                if (i == randomNumber)
                    return coord;
                i++;
            }

            return new Point(0, 0);
        }


        public bool IsInMap(int x, int y)
        {
            var walkableSquares = new List<Point>();
            for (var dY = 0; dY < _gameMap.GetUpperBound(1); dY++)
            {
                for (var dX = 0; dX < _gameMap.GetUpperBound(0); dX++)
                {
                    if (_model.DoorX != dX && _model.DoorY != dY && _gameMap[dX, dY] == 1)
                        walkableSquares.Add(new Point(dX, dY));
                }
            }

            if (walkableSquares.Contains(new Point(x, y)))
                return true;
            return false;
        }

        public void AddToMap(Item item)
        {
            AddItemToMap(item);
        }

        private void SetDefaultValue(int x, int y)
        {
            _gameMap[x, y] = 0;
            _userItemEffect[x, y] = 0;
            _itemHeightMap[x, y] = 0.0;

            if (x == Model.DoorX && y == Model.DoorY)
            {
                _gameMap[x, y] = 3;
            }
            else if (Model.SqState[x, y] == SquareState.Open)
            {
                _gameMap[x, y] = 1;
            }
            else if (Model.SqState[x, y] == SquareState.Seat)
            {
                _gameMap[x, y] = 2;
            }
        }

        public void UpdateMapForItem(Item item)
        {
            RemoveFromMap(item);
            AddToMap(item);
        }

        public void GenerateMaps(bool checkLines = true)
        {
            var maxX = 0;
            var maxY = 0;
            _coordinatedItems = new ConcurrentDictionary<Point, List<int>>();

            if (checkLines)
            {
                var items = _room.GetRoomItemHandler().GetFloor.ToArray();
                foreach (var item in items.ToList())
                {
                    if (item == null)
                        continue;

                    if (item.GetX > Model.MapSizeX && item.GetX > maxX)
                        maxX = item.GetX;
                    if (item.GetY > Model.MapSizeY && item.GetY > maxY)
                        maxY = item.GetY;
                }

                Array.Clear(items, 0, items.Length);
                items = null;
            }


            if (maxY > (Model.MapSizeY - 1) || maxX > (Model.MapSizeX - 1))
            {
                if (maxX < Model.MapSizeX)
                    maxX = Model.MapSizeX;
                if (maxY < Model.MapSizeY)
                    maxY = Model.MapSizeY;

                Model.SetMapsize(maxX + 7, maxY + 7);
                GenerateMaps(false);
                return;
            }

            if (maxX != StaticModel.MapSizeX || maxY != StaticModel.MapSizeY)
            {
                _userItemEffect = new byte[Model.MapSizeX, Model.MapSizeY];
                _gameMap = new byte[Model.MapSizeX, Model.MapSizeY];


                _itemHeightMap = new double[Model.MapSizeX, Model.MapSizeY];
                //if (modelRemap)
                //    Model.Generate(); //Clears model

                for (var line = 0; line < Model.MapSizeY; line++)
                {
                    for (var chr = 0; chr < Model.MapSizeX; chr++)
                    {
                        _gameMap[chr, line] = 0;
                        _userItemEffect[chr, line] = 0;

                        if (chr == Model.DoorX && line == Model.DoorY)
                        {
                            _gameMap[chr, line] = 3;
                        }
                        else if (Model.SqState[chr, line] == SquareState.Open)
                        {
                            _gameMap[chr, line] = 1;
                        }
                        else if (Model.SqState[chr, line] == SquareState.Seat)
                        {
                            _gameMap[chr, line] = 2;
                        }
                        else if (Model.SqState[chr, line] == SquareState.Pool)
                        {
                            _userItemEffect[chr, line] = 6;
                        }
                    }
                }
            }
            else
            {
                _userItemEffect = new byte[Model.MapSizeX, Model.MapSizeY];
                _gameMap = new byte[Model.MapSizeX, Model.MapSizeY];


                _itemHeightMap = new double[Model.MapSizeX, Model.MapSizeY];

                for (var line = 0; line < Model.MapSizeY; line++)
                {
                    for (var chr = 0; chr < Model.MapSizeX; chr++)
                    {
                        _gameMap[chr, line] = 0;
                        _userItemEffect[chr, line] = 0;

                        if (chr == Model.DoorX && line == Model.DoorY)
                        {
                            _gameMap[chr, line] = 3;
                        }
                        else if (Model.SqState[chr, line] == SquareState.Open)
                        {
                            _gameMap[chr, line] = 1;
                        }
                        else if (Model.SqState[chr, line] == SquareState.Seat)
                        {
                            _gameMap[chr, line] = 2;
                        }
                        else if (Model.SqState[chr, line] == SquareState.Pool)
                        {
                            _userItemEffect[chr, line] = 6;
                        }
                    }
                }
            }

            var tmpItems = _room.GetRoomItemHandler().GetFloor.ToArray();
            foreach (var item in tmpItems.ToList())
            {
                if (item == null)
                    continue;

                if (!AddItemToMap(item))
                    continue;
            }
            Array.Clear(tmpItems, 0, tmpItems.Length);
            tmpItems = null;

            if (_room.RoomBlockingEnabled == 0)
            {
                foreach (var user in _room.GetRoomUserManager().GetUserList().ToList())
                {
                    if (user == null)
                        continue;

                    user.SqState = _gameMap[user.X, user.Y];
                    _gameMap[user.X, user.Y] = 0;
                }
            }

            try
            {
                _gameMap[Model.DoorX, Model.DoorY] = 3;
            }
            catch { }
        }

        private bool ConstructMapForItem(Item item, Point coord)
        {
            try
            {
                if (coord.X > (Model.MapSizeX - 1))
                {
                    Model.AddX();
                    GenerateMaps();
                    return false;
                }

                if (coord.Y > (Model.MapSizeY - 1))
                {
                    Model.AddY();
                    GenerateMaps();
                    return false;
                }

                if (Model.SqState[coord.X, coord.Y] == SquareState.Blocked)
                {
                    Model.OpenSquare(coord.X, coord.Y, item.GetZ);
                }
                if (_itemHeightMap[coord.X, coord.Y] <= item.TotalHeight)
                {
                    _itemHeightMap[coord.X, coord.Y] = item.TotalHeight - _dynamicModel.SqFloorHeight[item.GetX, item.GetY];
                    _userItemEffect[coord.X, coord.Y] = 0;


                    switch (item.GetBaseItem().InteractionType)
                    {
                        case InteractionType.Pool:
                            _userItemEffect[coord.X, coord.Y] = 1;
                            break;
                        case InteractionType.NormalSkates:
                            _userItemEffect[coord.X, coord.Y] = 2;
                            break;
                        case InteractionType.IceSkates:
                            _userItemEffect[coord.X, coord.Y] = 3;
                            break;
                        case InteractionType.Lowpool:
                            _userItemEffect[coord.X, coord.Y] = 4;
                            break;
                        case InteractionType.Haloweenpool:
                            _userItemEffect[coord.X, coord.Y] = 5;
                            break;
                    }


                    //SwimHalloween
                    if (item.GetBaseItem().Walkable)    // If this item is walkable and on the floor, allow users to walk here.
                    {
                        if (_gameMap[coord.X, coord.Y] != 3)
                            _gameMap[coord.X, coord.Y] = 1;
                    }
                    else if (item.GetZ <= (Model.SqFloorHeight[item.GetX, item.GetY] + 0.1) && item.GetBaseItem().InteractionType == InteractionType.Gate && item.ExtraData == "1")// If this item is a gate, open, and on the floor, allow users to walk here.
                    {
                        if (_gameMap[coord.X, coord.Y] != 3)
                            _gameMap[coord.X, coord.Y] = 1;
                    }
                    else if (item.GetBaseItem().IsSeat || item.GetBaseItem().InteractionType == InteractionType.Bed || item.GetBaseItem().InteractionType == InteractionType.TentSmall)
                    {
                        _gameMap[coord.X, coord.Y] = 3;
                    }
                    else // Finally, if it's none of those, block the square.
                    {
                        if (_gameMap[coord.X, coord.Y] != 3)
                            _gameMap[coord.X, coord.Y] = 0;
                    }
                }

                // Set bad maps
                if (item.GetBaseItem().InteractionType == InteractionType.Bed || item.GetBaseItem().InteractionType == InteractionType.TentSmall)
                    _gameMap[coord.X, coord.Y] = 3;
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
            }
            return true;
        }

        public void AddCoordinatedItem(Item item, Point coord)
        {
            var items = new List<int>(); //mCoordinatedItems[CoordForItem];

            if (!_coordinatedItems.TryGetValue(coord, out items))
            {
                items = new List<int>();

                if (!items.Contains(item.Id))
                    items.Add(item.Id);

                if (!_coordinatedItems.ContainsKey(coord))
                    _coordinatedItems.TryAdd(coord, items);
            }
            else
            {
                if (!items.Contains(item.Id))
                {
                    items.Add(item.Id);
                    _coordinatedItems[coord] = items;
                }
            }
        }

        public List<Item> GetCoordinatedItems(Point coord)
        {
            var point = new Point(coord.X, coord.Y);
            var items = new List<Item>();

            if (_coordinatedItems.ContainsKey(point))
            {
                var ids = _coordinatedItems[point];
                items = GetItemsFromIds(ids);
                return items;
            }

            return new List<Item>();
        }

        public bool RemoveCoordinatedItem(Item item, Point coord)
        {
            var point = new Point(coord.X, coord.Y);
            if (_coordinatedItems != null && _coordinatedItems.ContainsKey(point))
            {
                ((List<int>)_coordinatedItems[point]).RemoveAll(x => x == item.Id);
                return true;
            }
            return false;
        }

        private void AddSpecialItems(Item item)
        {
            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.FootballGate:
                    //IsTrans = true;
                    _room.GetSoccer().RegisterGate(item);


                    var splittedExtraData = item.ExtraData.Split(':');

                    if (string.IsNullOrEmpty(item.ExtraData) || splittedExtraData.Length <= 1)
                    {
                        item.Gender = "M";
                        switch (item.Team)
                        {
                            case Team.Yellow:
                                item.Figure = "lg-275-93.hr-115-61.hd-207-14.ch-265-93.sh-305-62";
                                break;
                            case Team.Red:
                                item.Figure = "lg-275-96.hr-115-61.hd-180-3.ch-265-96.sh-305-62";
                                break;
                            case Team.Green:
                                item.Figure = "lg-275-102.hr-115-61.hd-180-3.ch-265-102.sh-305-62";
                                break;
                            case Team.Blue:
                                item.Figure = "lg-275-108.hr-115-61.hd-180-3.ch-265-108.sh-305-62";
                                break;
                        }
                    }
                    else
                    {
                        item.Gender = splittedExtraData[0];
                        item.Figure = splittedExtraData[1];
                    }
                    break;

                case InteractionType.Banzaifloor:
                    {
                        _room.GetBanzai().AddTile(item, item.Id);
                        break;
                    }

                case InteractionType.Banzaipyramid:
                    {
                        _room.GetGameItemHandler().AddPyramid(item, item.Id);
                        break;
                    }

                case InteractionType.Banzaitele:
                    {
                        _room.GetGameItemHandler().AddTeleport(item, item.Id);
                        item.ExtraData = "";
                        break;
                    }
                case InteractionType.Banzaipuck:
                    {
                        _room.GetBanzai().AddPuck(item);
                        break;
                    }

                case InteractionType.Football:
                    {
                        _room.GetSoccer().AddBall(item);
                        break;
                    }
                case InteractionType.FreezeTileBlock:
                    {
                        _room.GetFreeze().AddFreezeBlock(item);
                        break;
                    }
                case InteractionType.FreezeTile:
                    {
                        _room.GetFreeze().AddFreezeTile(item);
                        break;
                    }
                case InteractionType.Freezeexit:
                    {
                        _room.GetFreeze().AddExitTile(item);
                        break;
                    }
            }
        }

        private void RemoveSpecialItem(Item item)
        {
            switch (item.GetBaseItem().InteractionType)
            {
                case InteractionType.FootballGate:
                    _room.GetSoccer().UnRegisterGate(item);
                    break;
                case InteractionType.Banzaifloor:
                    _room.GetBanzai().RemoveTile(item.Id);
                    break;
                case InteractionType.Banzaipuck:
                    _room.GetBanzai().RemovePuck(item.Id);
                    break;
                case InteractionType.Banzaipyramid:
                    _room.GetGameItemHandler().RemovePyramid(item.Id);
                    break;
                case InteractionType.Banzaitele:
                    _room.GetGameItemHandler().RemoveTeleport(item.Id);
                    break;
                case InteractionType.Football:
                    _room.GetSoccer().RemoveBall(item.Id);
                    break;
                case InteractionType.FreezeTile:
                    _room.GetFreeze().RemoveFreezeTile(item.Id);
                    break;
                case InteractionType.FreezeTileBlock:
                    _room.GetFreeze().RemoveFreezeBlock(item.Id);
                    break;
                case InteractionType.Freezeexit:
                    _room.GetFreeze().RemoveExitTile(item.Id);
                    break;
            }
        }

        public bool RemoveFromMap(Item item, bool handleGameItem)
        {
            if (handleGameItem)
                RemoveSpecialItem(item);

            if (_room.GotSoccer())
                _room.GetSoccer().OnGateRemove(item);

            var isRemoved = false;
            foreach (var coord in item.GetCoords.ToList())
            {
                if (RemoveCoordinatedItem(item, coord))
                    isRemoved = true;
            }

            var conflictedName = new ConcurrentDictionary<Point, List<Item>>();
            foreach (var tile in item.GetCoords.ToList())
            {
                var point = new Point(tile.X, tile.Y);
                if (_coordinatedItems.ContainsKey(point))
                {
                    var ids = (List<int>)_coordinatedItems[point];
                    var items = GetItemsFromIds(ids);

                    if (!conflictedName.ContainsKey(tile))
                        conflictedName.TryAdd(tile, items);
                }

                SetDefaultValue(tile.X, tile.Y);
            }

            foreach (var coord in conflictedName.Keys.ToList())
            {
                if (!conflictedName.ContainsKey(coord))
                    continue;

                var subItems = (List<Item>)conflictedName[coord];
                foreach (var subItem in subItems.ToList())
                {
                    ConstructMapForItem(subItem, coord);
                }
            }


            conflictedName.Clear();
            conflictedName = null;


            return isRemoved;
        }

        public bool RemoveFromMap(Item item)
        {
            return RemoveFromMap(item, true);
        }

        public bool AddItemToMap(Item item, bool handleGameItem, bool newItem = true)
        {

            if (handleGameItem)
            {
                AddSpecialItems(item);

                switch (item.GetBaseItem().InteractionType)
                {
                    case InteractionType.FootballGoalRed:
                    case InteractionType.Footballcounterred:
                    case InteractionType.Banzaiscorered:
                    case InteractionType.Banzaigatered:
                    case InteractionType.Freezeredcounter:
                    case InteractionType.FreezeRedGate:
                        {
                            if (!_room.GetRoomItemHandler().GetFloor.Contains(item))
                                _room.GetGameManager().AddFurnitureToTeam(item, Team.Red);
                            break;
                        }
                    case InteractionType.FootballGoalGreen:
                    case InteractionType.Footballcountergreen:
                    case InteractionType.Banzaiscoregreen:
                    case InteractionType.Banzaigategreen:
                    case InteractionType.Freezegreencounter:
                    case InteractionType.FreezeGreenGate:
                        {
                            if (!_room.GetRoomItemHandler().GetFloor.Contains(item))
                                _room.GetGameManager().AddFurnitureToTeam(item, Team.Green);
                            break;
                        }
                    case InteractionType.FootballGoalBlue:
                    case InteractionType.Footballcounterblue:
                    case InteractionType.Banzaiscoreblue:
                    case InteractionType.Banzaigateblue:
                    case InteractionType.Freezebluecounter:
                    case InteractionType.FreezeBlueGate:
                        {
                            if (!_room.GetRoomItemHandler().GetFloor.Contains(item))
                                _room.GetGameManager().AddFurnitureToTeam(item, Team.Blue);
                            break;
                        }
                    case InteractionType.FootballGoalYellow:
                    case InteractionType.Footballcounteryellow:
                    case InteractionType.Banzaiscoreyellow:
                    case InteractionType.Banzaigateyellow:
                    case InteractionType.Freezeyellowcounter:
                    case InteractionType.FreezeYellowGate:
                        {
                            if (!_room.GetRoomItemHandler().GetFloor.Contains(item))
                                _room.GetGameManager().AddFurnitureToTeam(item, Team.Yellow);
                            break;
                        }
                    case InteractionType.Freezeexit:
                        {
                            _room.GetFreeze().AddExitTile(item);
                            break;
                        }
                    case InteractionType.Roller:
                        {
                            if (!_room.GetRoomItemHandler().GetRollers().Contains(item))
                                _room.GetRoomItemHandler().TryAddRoller(item.Id, item);
                            break;
                        }
                }
            }

            if (item.GetBaseItem().Type != 's')
                return true;

            foreach (var coord in item.GetCoords.ToList())
            {
                AddCoordinatedItem(item, new Point(coord.X, coord.Y));
            }

            if (item.GetX > (Model.MapSizeX - 1))
            {
                Model.AddX();
                GenerateMaps();
                return false;
            }

            if (item.GetY > (Model.MapSizeY - 1))
            {
                Model.AddY();
                GenerateMaps();
                return false;
            }

            var @return = true;

            foreach (var coord in item.GetCoords)
            {
                if (!ConstructMapForItem(item, coord))
                {
                    @return = false;
                }
                else
                {
                    @return = true;
                }
            }



            return @return;
        }


        public bool CanWalk(int x, int y, bool @override)
        {

            if (@override)
            {
                return true;
            }

            if (_room.GetRoomUserManager().GetUserForSquare(x, y) != null && _room.RoomBlockingEnabled == 0)
                return false;

            return true;
        }

        public bool AddItemToMap(Item item, bool newItem = true)
        {
            return AddItemToMap(item, true, newItem);
        }

        public bool ItemCanMove(Item item, Point moveTo)
        {
            var points = GetAffectedTiles(item.GetBaseItem().Length, item.GetBaseItem().Width, moveTo.X, moveTo.Y, item.Rotation).Values.ToList();

            if (points == null || points.Count == 0)
                return true;

            foreach (var coord in points)
            {

                if (coord.X >= Model.MapSizeX || coord.Y >= Model.MapSizeY)
                    return false;

                if (!SquareIsOpen(coord.X, coord.Y, false))
                    return false;

                continue;
            }

            return true;
        }

        public byte GetFloorStatus(Point coord)
        {
            if (coord.X > _gameMap.GetUpperBound(0) || coord.Y > _gameMap.GetUpperBound(1))
                return 1;

            return _gameMap[coord.X, coord.Y];
        }

        public void SetFloorStatus(int x, int y, byte status)
        {
            _gameMap[x, y] = status;
        }

        public double GetHeightForSquareFromData(Point coord)
        {
            if (coord.X > _dynamicModel.SqFloorHeight.GetUpperBound(0) ||
                coord.Y > _dynamicModel.SqFloorHeight.GetUpperBound(1))
                return 1;
            return _dynamicModel.SqFloorHeight[coord.X, coord.Y];
        }

        public bool CanRollItemHere(int x, int y)
        {
            if (!ValidTile(x, y))
                return false;

            if (Model.SqState[x, y] == SquareState.Blocked)
                return false;

            return true;
        }

        public bool SquareIsOpen(int x, int y, bool pOverride)
        {
            if ((_dynamicModel.MapSizeX - 1) < x || (_dynamicModel.MapSizeY - 1) < y)
                return false;

            return CanWalk(_gameMap[x, y], pOverride);
        }

        public bool GetHighestItemForSquare(Point square, out Item item)
        {
            var items = GetAllRoomItemForSquare(square.X, square.Y);
            item = null;
            double highestZ = -1;

            if (items != null && items.Count() > 0)
            {
                foreach (var uItem in items.ToList())
                {
                    if (uItem == null)
                        continue;

                    if (uItem.TotalHeight > highestZ)
                    {
                        highestZ = uItem.TotalHeight;
                        item = uItem;
                        continue;
                    }
                    else
                        continue;
                }
            }
            else
                return false;

            return true;
        }

        public double GetHeightForSquare(Point coord)
        {
            if (GetHighestItemForSquare(coord, out var rItem))
                if (rItem != null)
                    return rItem.TotalHeight;

            return 0.0;
        }

        public Point GetChaseMovement(Item item)
        {
            var distance = 99;
            var coord = new Point(0, 0);
            var iX = item.GetX;
            var iY = item.GetY;
            var x = false;

            foreach (var user in _room.GetRoomUserManager().GetRoomUsers())
            {
                if (user.X == item.GetX || item.GetY == user.Y)
                {
                    if (user.X == item.GetX)
                    {
                        var difference = Math.Abs(user.Y - item.GetY);
                        if (difference < distance)
                        {
                            distance = difference;
                            coord = user.Coordinate;
                            x = false;
                        }
                        else
                            continue;

                    }
                    else if (user.Y == item.GetY)
                    {
                        var difference = Math.Abs(user.X - item.GetX);
                        if (difference < distance)
                        {
                            distance = difference;
                            coord = user.Coordinate;
                            x = true;
                        }
                        else
                            continue;
                    }
                    else
                        continue;
                }
            }

            if (distance > 5)
                return item.GetSides().OrderBy(_ => Guid.NewGuid()).FirstOrDefault();
            if (x && distance < 99)
            {
                if (iX > coord.X)
                {
                    iX--;
                    return new Point(iX, iY);
                }
                else
                {
                    iX++;
                    return new Point(iX, iY);
                }
            }
            else if (!x && distance < 99)
            {
                if (iY > coord.Y)
                {
                    iY--;
                    return new Point(iX, iY);
                }
                else
                {
                    iY++;
                    return new Point(iX, iY);
                }
            }
            else
                return item.Coordinate;
        }

        public bool IsValidStep2(RoomUser user, Vector2D @from, Vector2D to, bool endOfPath, bool @override)
        {
            if (user == null)
                return false;

            if (!ValidTile(to.X, to.Y))
                return false;

            if (@override)
                return true;

            /*
             * 0 = blocked
             * 1 = open
             * 2 = last step
             * 3 = door
             * */

            var items = _room.GetGameMap().GetAllRoomItemForSquare(to.X, to.Y);
            if (items.Count > 0)
            {
                var hasGroupGate = items.ToList().Count(x => x.GetBaseItem().InteractionType == InteractionType.GuildGate) > 0;
                if (hasGroupGate)
                {
                    var I = items.FirstOrDefault(x => x.GetBaseItem().InteractionType == InteractionType.GuildGate);
                    if (I != null)
                    {
                        if (!PlusEnvironment.GetGame().GetGroupManager().TryGetGroup(I.GroupId, out var @group))
                            return false;

                        if (user.GetClient() == null || user.GetClient().GetHabbo() == null)
                            return false;

                        if (@group.IsMember(user.GetClient().GetHabbo().Id))
                        {
                            I.InteractingUser = user.GetClient().GetHabbo().Id;
                            I.ExtraData = "1";
                            I.UpdateState(false, true);

                            I.RequestUpdate(4, true);

                            return true;
                        }
                        else
                        {
                            if (user.Path.Count > 0)
                                user.Path.Clear();
                            user.PathRecalcNeeded = false;
                            return false;
                        }
                    }
                }
            }

            var chair = false;
            double highestZ = -1;
            foreach (var item in items.ToList())
            {
                if (item == null)
                    continue;

                if (item.GetZ < highestZ)
                {
                    chair = false;
                    continue;
                }

                highestZ = item.GetZ;
                if (item.GetBaseItem().IsSeat)
                    chair = true;
            }

            if ((_gameMap[to.X, to.Y] == 3 && !endOfPath && !chair) || (_gameMap[to.X, to.Y] == 0) || (_gameMap[to.X, to.Y] == 2 && !endOfPath))
            {
                if (user.Path.Count > 0)
                    user.Path.Clear();
                user.PathRecalcNeeded = true;
            }

            var heightDiff = SqAbsoluteHeight(to.X, to.Y) - SqAbsoluteHeight(@from.X, @from.Y);
            if (heightDiff > 1.5 && !user.RidingHorse)
                return false;

            //Check this last, because ya.
            var userx = _room.GetRoomUserManager().GetUserForSquare(to.X, to.Y);
            if (userx != null)
            {
                if (!userx.IsWalking && endOfPath)
                    return false;
            }
            return true;
        }
    
        public bool IsValidStep(Vector2D from, Vector2D to, bool endOfPath, bool overriding, bool roller = false)
        {
            if (!ValidTile(to.X, to.Y))
                return false;

            if (overriding)
                return true;

            /*
             * 0 = blocked
             * 1 = open
             * 2 = last step
             * 3 = door
             * */

            if (_room.RoomBlockingEnabled == 0 && SquareHasUsers(to.X, to.Y))
                return false;

            var items = _room.GetGameMap().GetAllRoomItemForSquare(to.X, to.Y);
            if (items.Count > 0)
            {
                var hasGroupGate = items.ToList().Count(x => x != null && x.GetBaseItem().InteractionType == InteractionType.GuildGate) > 0;
                if (hasGroupGate)
                    return true;
            }

            if ((_gameMap[to.X, to.Y] == 3 && !endOfPath) || _gameMap[to.X, to.Y] == 0 || (_gameMap[to.X, to.Y] == 2 && !endOfPath))
                return false;

            if (!roller)
            {
                var heightDiff = SqAbsoluteHeight(to.X, to.Y) - SqAbsoluteHeight(from.X, from.Y);
                if (heightDiff > 1.5)
                    return false;
            }

            return true;
        }

        public static bool CanWalk(byte state, bool overriding)
        {
            if (!overriding)
            {
                if (state == 3)
                    return true;
                if (state == 1)
                    return true;

                return false;
            }
            return true;
        }

        public bool ItemCanBePlaced(int x, int y)
        {
            if (_dynamicModel.MapSizeX - 1 < x || _dynamicModel.MapSizeY - 1 < y ||
                (x == _dynamicModel.DoorX && y == _dynamicModel.DoorY))
                return false;

            return _gameMap[x, y] == 1;
        }

        public double SqAbsoluteHeight(int x, int y)
        {
            var points = new Point(x, y);


            if (_coordinatedItems.TryGetValue(points, out var ids))
            {
                var items = GetItemsFromIds(ids);

                return SqAbsoluteHeight(x, y, items);
            }
            else
                return _dynamicModel.SqFloorHeight[x, y];
            /*
            if (mCoordinatedItems.ContainsKey(Points))
            {
                List<Item> Items = new List<Item>();
                foreach (Item Item in mCoordinatedItems[Points].ToArray())
                {
                    if (!Items.Contains(Item))
                        Items.Add(Item);
                }
                return SqAbsoluteHeight(X, Y, Items);
            }*/
        }

        public double SqAbsoluteHeight(int x, int y, List<Item> itemsOnSquare)
        {
            try
            {
                var deduct = false;
                double highestStack = 0;
                var deductable = 0.0;

                if (itemsOnSquare != null && itemsOnSquare.Count > 0)
                {
                    foreach (var item in itemsOnSquare.ToList())
                    {
                        if (item == null)
                            continue;

                        if (item.TotalHeight > highestStack)
                        {
                            if (item.GetBaseItem().IsSeat || item.GetBaseItem().InteractionType == InteractionType.Bed || item.GetBaseItem().InteractionType == InteractionType.TentSmall)
                            {
                                deduct = true;
                                deductable = item.GetBaseItem().Height;
                            }
                            else
                                deduct = false;
                            highestStack = item.TotalHeight;
                        }
                    }
                }

                double floorHeight = Model.SqFloorHeight[x, y];
                var stackHeight = highestStack - Model.SqFloorHeight[x, y];

                if (deduct)
                    stackHeight -= deductable;

                if (stackHeight < 0)
                    stackHeight = 0;

                return (floorHeight + stackHeight);
            }
            catch (Exception e)
            {
                ExceptionLogger.LogException(e);
                return 0;
            }
        }

        public bool ValidTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= Model.MapSizeX || y >= Model.MapSizeY)
            {
                return false;
            }

            return true;
        }

        public static Dictionary<int, ThreeDCoord> GetAffectedTiles(int length, int width, int posX, int posY, int rotation)
        {
            var x = 0;

            var pointList = new Dictionary<int, ThreeDCoord>();

            if (length > 1)
            {
                if (rotation == 0 || rotation == 4)
                {
                    for (var i = 1; i < length; i++)
                    {
                        pointList.Add(x++, new ThreeDCoord(posX, posY + i, i));

                        for (var j = 1; j < width; j++)
                        {
                            pointList.Add(x++, new ThreeDCoord(posX + j, posY + i, (i < j) ? j : i));
                        }
                    }
                }
                else if (rotation == 2 || rotation == 6)
                {
                    for (var i = 1; i < length; i++)
                    {
                        pointList.Add(x++, new ThreeDCoord(posX + i, posY, i));

                        for (var j = 1; j < width; j++)
                        {
                            pointList.Add(x++, new ThreeDCoord(posX + i, posY + j, (i < j) ? j : i));
                        }
                    }
                }
            }

            if (width > 1)
            {
                if (rotation == 0 || rotation == 4)
                {
                    for (var i = 1; i < width; i++)
                    {
                        pointList.Add(x++, new ThreeDCoord(posX + i, posY, i));

                        for (var j = 1; j < length; j++)
                        {
                            pointList.Add(x++, new ThreeDCoord(posX + i, posY + j, (i < j) ? j : i));
                        }
                    }
                }
                else if (rotation == 2 || rotation == 6)
                {
                    for (var i = 1; i < width; i++)
                    {
                        pointList.Add(x++, new ThreeDCoord(posX, posY + i, i));

                        for (var j = 1; j < length; j++)
                        {
                            pointList.Add(x++, new ThreeDCoord(posX + j, posY + i, (i < j) ? j : i));
                        }
                    }
                }
            }

            return pointList;
        }

        public List<Item> GetItemsFromIds(List<int> input)
        {
            if (input == null || input.Count == 0)
                return new List<Item>();

            var items = new List<Item>();

            lock (input)
            {
                foreach (var id in input.ToList())
                {
                    var itm = _room.GetRoomItemHandler().GetItem(id);
                    if (itm != null && !items.Contains(itm))
                        items.Add(itm);
                }
            }

            return items.ToList();
        }

        public List<Item> GetRoomItemForSquare(int pX, int pY, double minZ)
        {
            var itemsToReturn = new List<Item>();


            var coord = new Point(pX, pY);
            if (_coordinatedItems.ContainsKey(coord))
            {
                var itemsFromSquare = GetItemsFromIds((List<int>)_coordinatedItems[coord]);

                foreach (var item in itemsFromSquare)
                    if (item.GetZ > minZ)
                        if (item.GetX == pX && item.GetY == pY)
                            itemsToReturn.Add(item);
            }

            return itemsToReturn;
        }

        public List<Item> GetRoomItemForSquare(int pX, int pY)
        {
            var coord = new Point(pX, pY);
            //List<RoomItem> itemsFromSquare = new List<RoomItem>();
            var itemsToReturn = new List<Item>();

            if (_coordinatedItems.ContainsKey(coord))
            {
                var itemsFromSquare = GetItemsFromIds((List<int>)_coordinatedItems[coord]);

                foreach (var item in itemsFromSquare)
                {
                    if (item.Coordinate.X == coord.X && item.Coordinate.Y == coord.Y)
                        itemsToReturn.Add(item);
                }
            }

            return itemsToReturn;
        }

        public List<Item> GetAllRoomItemForSquare(int x, int y)
        {
            var coord = new Point(x, y);

            var items = new List<Item>();


            if (_coordinatedItems.TryGetValue(coord, out var ids))
                items = GetItemsFromIds(ids);
            else
                items = new List<Item>();

            return items;
        }

        public bool SquareHasUsers(int x, int y)
        {
            return MapGotUser(new Point(x, y));
        }


        public static bool TilesTouching(int x1, int y1, int x2, int y2)
        {
            if (!(Math.Abs(x1 - x2) > 1 || Math.Abs(y1 - y2) > 1)) return true;
            if (x1 == x2 && y1 == y2) return true;
            return false;
        }

        public static int TileDistance(int x1, int y1, int x2, int y2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(y1 - y2);
        }

        public DynamicRoomModel Model => _dynamicModel;

        public RoomModel StaticModel => _model;

        public byte[,] EffectMap => _userItemEffect;

        public byte[,] GameMap => _gameMap;

        public void Dispose()
        {
            _userMap.Clear();
            _dynamicModel.Destroy();
            _coordinatedItems.Clear();

            Array.Clear(_gameMap, 0, _gameMap.Length);
            Array.Clear(_userItemEffect, 0, _userItemEffect.Length);
            Array.Clear(_itemHeightMap, 0, _itemHeightMap.Length);

            _userMap = null;
            _gameMap = null;
            _userItemEffect = null;
            _itemHeightMap = null;
            _coordinatedItems = null;

            _dynamicModel = null;
            _room = null;
            _model = null;
        }
    }
}