using UnityEngine;

public class turret : Nemico
{
	void Start() {
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










