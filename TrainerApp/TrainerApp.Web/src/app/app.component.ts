import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router'; // standalone модуль для маршрутизації

@Component({
  selector: 'app-root',
  standalone: true,         // робимо компонент standalone
  imports: [RouterOutlet],  // імпортуємо всі потрібні модулі (тут лише маршрутизацію)
 templateUrl: './app.html',
styleUrls: ['./app.scss'],

})
export class AppComponent {}
