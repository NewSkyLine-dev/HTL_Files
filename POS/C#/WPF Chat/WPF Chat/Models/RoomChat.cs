// ---------------------------------------------------------------------------------------------------
// <auto-generated>
// This code was generated by LinqToDB scaffolding tool (https://github.com/linq2db/linq2db).
// Changes to this file may cause incorrect behavior and will be lost if the code is regenerated.
// </auto-generated>
// ---------------------------------------------------------------------------------------------------

using LinqToDB.Mapping;

#pragma warning disable 1573, 1591
#nullable enable

namespace DataModel
{
	[Table("room_chat")]
	public class RoomChat
	{
		[Column("room_id")] public long? RoomId { get; set; } // integer
		[Column("chat_id")] public long? ChatId { get; set; } // integer

		#region Associations
		/// <summary>
		/// FK_room_chat_0_0
		/// </summary>
		[Association(ThisKey = nameof(ChatId), OtherKey = nameof(DataModel.Chat.ChatId))]
		public Chat? Chat { get; set; }

		/// <summary>
		/// FK_room_chat_1_0
		/// </summary>
		[Association(ThisKey = nameof(RoomId), OtherKey = nameof(DataModel.Room.RoomId))]
		public Room? Room { get; set; }
		#endregion
	}
}
