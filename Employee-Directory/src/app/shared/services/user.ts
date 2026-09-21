import { Injectable, signal } from '@angular/core';
import { UserModel } from '../../models/user/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private userSignal = signal<UserModel | null>(null);

  user = this.userSignal.asReadonly();

  setUser(user: UserModel){
    this.userSignal.set(user);
  }

  clearUser(){
    this.userSignal.set(null);
  }
}