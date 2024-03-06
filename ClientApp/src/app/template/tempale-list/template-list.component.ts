import { Component } from '@angular/core';
import { TemplateAddComponent } from '../template-add/template-add/template-add.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-template-list',
  templateUrl: './template-list.component.html',
  styleUrl: './template-list.component.scss'
})
export class TemplateListComponent {
  constructor(
    public dialog: MatDialog,
) {
}

    public ngOnInit(): void {

  }

  openDialog(): void {


    const dialogRef = this.dialog.open(TemplateAddComponent, {
        width: '800px',
        height: '100vh',
        position: {
            top: '0px',
            right: '0px',
        },
        panelClass: [
            'animate__animated',
            'animate__slideInRight',
            'no-border-wrapper',
        ],
        autoFocus: false,
        disableClose: true,
    });
  }

}
