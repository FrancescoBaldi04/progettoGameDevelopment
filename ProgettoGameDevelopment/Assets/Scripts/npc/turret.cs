using UnityEngine;

public class turret : Nemico
{
	private float timer=1.0f;
	private int obstacleLayerMask;
    protected override void Awake()
    {
        base.Awake();

		obstacleLayerMask = LayerMask.GetMask("ground");
    }
	
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
			case Stato.waiting: {
				timer-=Time.deltaTime;
				if (timer<=0 && parassita.StatoAttuale == Parassita.Stato.possessing) {
						timer=1.0f;
						this.StatoAttuale=Stato.shooting;
				}
				
				break;
			}
			
			case Stato.shooting: {
				timer-=Time.deltaTime;
				Vector2 myPosition=transform.position;
				Vector2 parassitaPosition=parassita.transform.position;
				
				float distance=Vector2.Distance(myPosition, parassitaPosition);
				Vector2 directionToParassita=(parassitaPosition-myPosition).normalized;

				RaycastHit2D hit=Physics2D.Raycast(myPosition, directionToParassita, distance, obstacleLayerMask);
				
				if (hit.collider!=null) {
					this.StatoAttuale=Stato.waiting;
					timer=1.0f;
				} else if (timer<=0) {
					Shoot();
					timer=1.0f;
				}
				
				break;
			}
		}
	}
}










