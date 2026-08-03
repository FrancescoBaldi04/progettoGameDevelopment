using UnityEngine;

public class scientistminiboss : MonoBehaviour
{
    public enum Stato {escaping, catching};
    private Stato StatoAttuale;
    private int HitPoints=600;
	private bool isDying = false;
	[SerializeField] private Parassita parassita; // assegnare il gameObject del parassita nell'inspector una volta creato l'oggetto corrispondente!

    void Start()
    {
        if (parassita.StatoAttuale==Parassita.Stato.possessing) 
	{	
		StatoAttuale=Stato.escaping;
	} else {
		StatoAttuale=Stato.catching;
	}
    }
    
    void Update()
    {
        if (HitPoints<=0 && !isDying) {
			isDying = true;
			Destroy(gameObject, 1.5f);
		}
	switch (StatoAttuale)
	{
		case Stato.catching:
			
			if (parassita.StatoAttuale==Parassita.Stato.possessing)
			{
				StatoAttuale=Stato.escaping;
			}
			break;
		case Stato.escaping:
			
			if (parassita.StatoAttuale!=Parassita.Stato.possessing)
			{
				StatoAttuale=Stato.catching;
			}
			break;
	}
    }
}









