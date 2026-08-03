using UnityEngine;

public class turret : MonoBehaviour
{
    public enum Stato {waiting, shooting};
    private Stato StatoAttuale;
	[SerializeField] private Parassita parassita; // assegnare il gameObject del parassita nell'inspector una volta creato l'oggetto corrispondente!
	
    void Start()
    {
        if (parassita.StatoAttuale == Parassita.Stato.possessing) 
	{	
		this.StatoAttuale=Stato.shooting;
	} else {
		this.StatoAttuale=Stato.waiting;
	}
    }
    
    void Update(){
	switch (StatoAttuale){
		case Stato.waiting:
			if (parassita.StatoAttuale == Parassita.Stato.possessing){
				this.StatoAttuale=Stato.shooting;
			}	
			break;
		
		case Stato.shooting:
			
			if (parassita.StatoAttuale != Parassita.Stato.possessing){
				this.StatoAttuale=Stato.waiting;
			}
			break;
		}
    }
}










