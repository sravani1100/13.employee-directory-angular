import { Injectable, signal } from '@angular/core';
import { ToastModel } from '../../models/toast/toast.model';

@Injectable({
  providedIn:'root'
})
export class ToastService {

  private toastList = signal<ToastModel[]>([]);

  toasts = this.toastList.asReadonly();

  show(
    message:string,
    type:'success'|'error'|'info' = 'info'
  ){

    const id = Date.now();

    this.toastList.update(list => [
      ...list,
      {
        id,
        message,
        type
      }
    ]);

    setTimeout(()=>{
      this.remove(id);
    },3000);
  }

  remove(id:number){

    this.toastList.update(list =>
      list.filter(t=>t.id !== id)
    );

  }
}