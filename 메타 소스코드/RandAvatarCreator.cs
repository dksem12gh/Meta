using Cysharp.Threading.Tasks;
using SpaceHub.Conference;
using UnityEngine;
using System.Threading;
using Avatar = SpaceHub.Conference.Avatar;

namespace Rapa
{
    public class RandAvatarCreator : MonoBehaviour
    {        
        [SerializeField] AvatarCustomization m_avatarCustomization = null;
        //[SerializeField] PlayerActionManager m_playerActionManager = null;

        private void Start()
        {
            m_avatarCustomization.ReleaseAvatar();            
            CustomizationData.Instance.LoadFromPlayerPrefs();
            StartCoroutine(m_avatarCustomization.CreateAvatar());
        }
    }
}