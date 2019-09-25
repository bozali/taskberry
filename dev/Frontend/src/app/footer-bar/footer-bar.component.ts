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
    this.privacyPolicySelected = false;
  }

  public SelectedTabChanged(tabName) {
    switch (tabName) {
      case 'Impressum':
        if(!this.imprintSelected) {
          window.open('https://www.atiwonline.de/#openModal');
          this.imprintSelected = true;
        }
        break;
      case 'Datenschutzerklärung':
          if (!this.privacyPolicySelected) {
            window.open('https://www.atiwonline.de/admin/tool/policy/viewall.php');
            this.imprintSelected = true;
          }
          break;
    }
  }

  public SetImprintActive() {
    this.imprintSelected = true;
  }

  public SetPrivacyPolicyActive() {
    this.privacyPolicySelected = true;
  }

}
