import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {

  title = 'Simple Workflow management systems';
  all = ['Task 1', 'Task 2', 'Task 3', 'Task 4'];
  even3 = ['Task 10', 'Task 11'];
  even = ['Task 19', 'Task 20', 'Task 21', 'Task 22', 'Task 23'];

  constructor() { }

  ngOnInit() {


  }



}
