import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-footer-bar',
  templateUrl: './footer-bar.component.html',
  styleUrls: ['./footer-bar.component.scss']
})
export class FooterBarComponent implements OnInit {
  public imprintSelected = false;
  public privacyPolicySelected = false;

  constructor() { }

  ngOnInit() {
    this.imprintSelected = false;
    this.imprintSelected = false;
  }

  public SelectedTabChanged(tabName) {
    switch (tabName) {
      case 'Impressum':
        if(this.imprintSelected) {
          window.open('https://www.atiwonline.de/#openModal');
          this.imprintSelected = true;
        }
        break;
      case 'Datenschutzerklärung':
          if(this.privacyPolicySelected) {
            window.open('https://www.atiwonline.de/admin/tool/policy/viewall.php?returnurl=https%3A%2F%2Fwww.atiwonline.de%2F');
            this.imprintSelected = true;
          }
          break;
    }
  }

}
