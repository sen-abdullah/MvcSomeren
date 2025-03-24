using Microsoft.AspNetCore.Mvc;

namespace MvcSomeren.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        public int RoomSize { get; set; }
        public string RoomType { get; set; }
        public string Building { get; set; }
        public string Floor { get; set; }
        public Room()
        {
            this.RoomId = 0;
            this.RoomNumber = 0;
            this.RoomSize = 0;
            this.RoomType = "";
            this.Building = "";
            this.Floor = "";
        }

        public Room(int roomId, int roomNumber, int roomSize, string roomType, string building, string floor)
        {
            this.RoomId = roomId;
            this.RoomNumber = roomNumber;
            this.RoomSize = roomSize;
            this.RoomType = roomType;
            this.Building = building;
            this.Floor = floor;
        }
    }
}
