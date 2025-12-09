import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { CardInterface } from '../interfaces/cards.interface';


@Injectable({
  providedIn: 'root'
})
export class CardServiceService {

  private allCards: CardInterface[] = []; // Tu arreglo completo de tarjetas
  private cardsPerPage = 4; // Número de tarjetas a cargar por página

  

  initCards():void{
    this.allCards=[];
  }
  setCards(cards: CardInterface[]): void {
    this.allCards = [...this.allCards, ...cards];
  }

  getCards(page: number): Observable<CardInterface[]> {
    const startIndex = page * this.cardsPerPage;
    const endIndex = startIndex + this.cardsPerPage;
    const slicedCards = this.allCards.slice(startIndex, endIndex);

    return of(slicedCards);
  }
}
