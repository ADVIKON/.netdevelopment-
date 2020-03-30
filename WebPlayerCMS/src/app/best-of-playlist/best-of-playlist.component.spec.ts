import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BestOfPlaylistComponent } from './best-of-playlist.component';

describe('BestOfPlaylistComponent', () => {
  let component: BestOfPlaylistComponent;
  let fixture: ComponentFixture<BestOfPlaylistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BestOfPlaylistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BestOfPlaylistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
