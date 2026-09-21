import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleCardComponent } from './role-card';

describe('RoleCardComponent', () => {
  let component: RoleCardComponent;
  let fixture: ComponentFixture<RoleCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoleCardComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(RoleCardComponent);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
