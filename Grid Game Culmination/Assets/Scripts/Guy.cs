using UnityEditor.SceneManagement;

namespace DefaultNamespace
{
    public class Guy : AbstractCharacter
    {
        void Awake()
        {
            Attacks[0] = gameObject.AddComponent<BasicGuyAttack>();
            Attacks[1] = gameObject.AddComponent<SpecialBlaster>();
        }
    }
}