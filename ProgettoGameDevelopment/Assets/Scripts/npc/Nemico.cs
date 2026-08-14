using UnityEngine;

public class Nemico : MonoBehaviour
{
    [SerializeField] protected int HitPoints = 60;

    public void PrendiDanno(int danno)
    {
        HitPoints -= danno;

        Debug.Log(name + " ha ricevuto " + danno + " danni. HP: " + HitPoints);

        if (HitPoints <= 0)
        {
            // qui eventualmente gestisci la morte
            Destroy(gameObject);
        }
    }
}