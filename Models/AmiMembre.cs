using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace NetAtlas_The_True_Project.Models
{
    public class AmiMembre
    {
        public AmiMembre()
        {

        }
        public int RequestedById { get; set; }
        public int RequestedToId { get; set; }
        [JsonIgnore]
        public virtual Membre RequestedBy { get; set; }
        [JsonIgnore]
        public virtual Membre RequestedTo { get; set; }

        /*public DateTime? RequestTime { get; set; }

        public DateTime? BecameFriendsTime { get; set; }*/

        public FriendRequestFlag FriendRequestFlag { get; set; }

        [NotMapped]
        public bool Approved => FriendRequestFlag == FriendRequestFlag.Approved;
        [NotMapped]
        public bool Rejected => FriendRequestFlag == FriendRequestFlag.Rejected;

        public void AddFriendRequest(Membre user, Membre friendUser)
        {
            var friendRequest = new AmiMembre()
            {
                RequestedBy = user,
                RequestedTo = friendUser,
                //RequestTime = DateTime.Now,
                FriendRequestFlag = FriendRequestFlag.None
            };
            user.SentFriendRequests.Add(friendRequest);
        }
    }

    public enum FriendRequestFlag
    {
        None,
        Approved,
        Rejected
    };
}
