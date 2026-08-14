using UnityEngine;

public class scientist : Nemico
{
    public enum Stato {escaping, catching, possessed};
    public Stato StatoAttuale { get; private set; }
	private bool isDying = false;
	[SerializeField] private Parassita parassita; // assegnare il gameObject del parassita nell'inspector una volta creato l'oggetto corrispondente!

    void Start(){
        if (parassita.StatoAttuale==Parassita.Stato.possessing) {	
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
		/*case Stato.catching:
			
			if ('colpiti dal Parassita')
			{
				this.StatoAttuale=Stato.possessed;
				this.HitPoints=60;
			}
			if (parassita.StatoAttuale==Parassita.Stato.possessing && this.StatoAttuale!=Stato.possessed)
			{
				this.StatoAttuale=Stato.escaping;
			}	
			break;*/
		
		case Stato.escaping:
			
			if (parassita.StatoAttuale!=Parassita.Stato.possessing)
			{
				StatoAttuale=Stato.catching;
			}
			break;
		
		case Stato.possessed:
			break;
	}
    }
}









