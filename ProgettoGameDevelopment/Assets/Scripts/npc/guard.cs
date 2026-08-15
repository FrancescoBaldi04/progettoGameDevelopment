using UnityEngine;
 
public class guard : Nemico
{
	private bool isDying = false;
	
	void Start() {
        if (parassita.StatoAttuale==Parassita.Stato.possessing) {	
			StatoAttuale=Stato.positioning;
		} else {
			StatoAttuale=Stato.escaping;
		}
	}

	void Update() {
        if (HitPoints<=0 && !isDying) {
			isDying = true;
			Destroy(gameObject, 1.5f); // in caso volessimo disabilitare commponenti o eseguire un'animazione di morte forse è meglio usare una coroutine
		}
	
		if (isDying) return;

		switch (StatoAttuale){
			/*case Stato.escaping:
				if ('colpiti dal Parassita')
				{
					this.StatoAttuale=Stato.possessed;
					this.HitPoints=60;
				}
				if (parassita.StatoAttuale==Parassita.Stato.possessing && this.StatoAttuale!=Stato.possessed)
				{
					this.StatoAttuale=Stato.positioning;
				}	
				break;
			
			case Stato.positioning:
				if ('raggiunge la posizione')
				{
					this.StatoAttuale=Stato.shooting;
				}
				if (parassita.StatoAttuale!=Parassita.Stato.possessing)
				{
					this.StatoAttuale=Stato.escaping;
				}
				break;*/
			
			case Stato.shooting:
				break;
			
			case Stato.possessed:
				break;
		}
	}
}
