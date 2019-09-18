import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardMyBoardComponent } from './dashboard-my-board.component';

describe('DashboardMyBoardComponent', () => {
  let component: DashboardMyBoardComponent;
  let fixture: ComponentFixture<DashboardMyBoardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardMyBoardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardMyBoardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
