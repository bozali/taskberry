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
          window.open('https://www.atiwonline.de/#openModal'); // Modal open

        break;
      case 'Datenschutzerklärung':
            window.open('https://www.atiwonline.de/admin/tool/policy/viewall.php');
          break;
          case 'ATIW':
          window.open('https://atiw.de/');
          break;
    }
  }

}
