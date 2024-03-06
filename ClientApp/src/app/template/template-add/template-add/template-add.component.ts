import { Component, EventEmitter, Output } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-template-add',
  templateUrl: './template-add.component.html',
  styleUrl: './template-add.component.scss'
})
export class TemplateAddComponent {
  @Output() closeEvent = new EventEmitter<void>();

  constructor(
    private dialogRef: MatDialogRef<TemplateAddComponent>,
    
  ) {

  }

  closeUserAdd() {
    this.closeEvent.emit();
}

  public countries: Array<{ text: string; value: number }> = [
    { text: "USA", value: 1 },
    { text: "Canada", value: 2 },
    { text: "UK", value: 3 },
    { text: "Australia", value: 4 },
    { text: "Germany", value: 5 },
    { text: "France", value: 6 },
  ];

  public autocompleteData: Array<{ text: string; value: string }> = [
    { text: "Albania", value: "Alb" },
    { text: "Andorra", value: "And" },
    { text: "Armenia", value: "Arm" },
    { text: "Austria", value: "Aus" },
    { text: "Azerbaijan", value: "Aze" },
  ];

  public multiselectData: Array<{ text: string; value: number }> = [
    { text: "Small", value: 1 },
    { text: "Medium", value: 2 },
    { text: "Large", value: 3 },
  ];

  public checkboxData = [
    { label: 'Item 1', value: 1 },
    { label: 'Item 2', value: 2 },
    { label: 'Item 3', value: 3 }
  ];

  roleItem: string[] = ["value", "value-2", "value-3"];

  closeDialog(status: boolean = false) {
    this.dialogRef.close(status);
  }

}
