import { Component, OnInit } from '@angular/core';
import { UselessTask } from '../models/UselessTask';
import { HttpClient, HttpParams } from '@angular/common/http';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-polling',
  templateUrl: './polling.component.html',
  styleUrls: ['./polling.component.css']
})
export class PollingComponent implements OnInit {

  title = 'labo.signalr.ng';
  tasks: UselessTask[] = [];
  taskname: string = "";

  constructor(private http:HttpClient){}

  ngOnInit(): void {
    this.updateTasks();
  }

  async complete(id: number) {
    // TODO On invoke la méthode pour compléter une tâche sur le serveur (Contrôleur d'API)
    let x = await lastValueFrom(this.http.get<any>("https://localhost:7289/api/UselessTasks/Complete/" + id));
    console.log(x);
  }

  async addtask() {
    // TODO On invoke la méthode pour ajouter une tâche sur le serveur (Contrôleur d'API)
    let x = await lastValueFrom(this.http.post<any>("https://localhost:7289/api/UselessTasks/Add?taskText=" + this.taskname, null));
    this.tasks.push(x);
    console.log(this.tasks);
  }

  async updateTasks() {
    // TODO: Faire une première implémentation simple avec un appel au serveur pour obtenir la liste des tâches
    let x = await lastValueFrom(this.http.get<UselessTask[]>("https://localhost:7289/api/UselessTasks/GetAll"));
    this.tasks = x;
    
    // TODO: UNE FOIS QUE VOUS AVEZ TESTER AVEC DEUX CLIENTS: Utiliser le polling pour mettre la liste de tasks à jour chaque seconde
    setTimeout(() => {this.updateTasks()}, 1000);
  }
}
