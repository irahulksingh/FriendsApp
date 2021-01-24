import { ConditionalExpr } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_service/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model : any ={}
  loggedin: boolean | undefined;
  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  login(){
   this.accountService.login(this.model).subscribe(response =>{
     console.log(response);
    this.loggedin=true;
   }, error =>{
     console.log (error);
   })
  }

}
