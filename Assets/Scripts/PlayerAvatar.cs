using UnityEngine;
using UnityEngine.UI;

public class PlayerAvator : Photon.MonoBehaviour
{
	PhotonView m_photonView;

    void Start() {
        m_photonView = GetComponent<PhotonView> ();
    }

    public void TakeDamage(GameObject i_projectile) {
        Debug.Log (string.Format("{0}に攻撃が当たった", this.gameObject.name));

        if(!m_photonView.isMine) {
            return;
        }

        // 所有権の移譲
        i_projectile.GetComponent<PhotonView> ().TransferOwnership (PhotonNetwork.player.ID);
        PhotonNetwork.Destroy (i_projectile);
        PhotonNetwork.Destroy (this.gameObject);
    }

}