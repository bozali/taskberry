import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer-bar',
  templateUrl: './footer-bar.component.html',
  styleUrls: ['./footer-bar.component.scss']
})
export class FooterBarComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

  public SelectedTabChanged(tabName) {
    switch (tabName) {
      case 'Impressum':
        window.open('https://www.atiwonline.de/#openModal');
      break;
      case 'Datenschutzerklärung':
          window.open('https://www.atiwonline.de/admin/tool/policy/viewall.php?returnurl=https%3A%2F%2Fwww.atiwonline.de%2F');
      break;
    }
  }

}
