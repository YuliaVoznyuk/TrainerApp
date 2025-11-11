import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router'; // standalone модуль для маршрутизації

@Component({
  selector: 'app-root',
  standalone: true,         // робимо компонент standalone
  imports: [RouterOutlet],  // імпортуємо всі потрібні модулі (тут лише маршрутизацію)
  template: `<router-outlet></router-outlet>`
})
export class AppComponent {}
