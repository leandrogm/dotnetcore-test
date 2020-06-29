import { Component, ViewEncapsulation, ViewChild, Inject } from '@angular/core';
import { ToolbarService, LinkService, ImageService, QuickToolbarService } from '@syncfusion/ej2-angular-richtexteditor';
import { HtmlEditorService, RichTextEditorComponent } from '@syncfusion/ej2-angular-richtexteditor';
import { HttpClient } from '@angular/common/http';
import Tribute from 'tributejs';


@Component({
  selector: 'rich-text',
  styleUrls: ['./rich-text.component.css'],
  templateUrl: './rich-text.component.html',
  encapsulation: ViewEncapsulation.None,
  providers: [ToolbarService, LinkService, ImageService, HtmlEditorService, QuickToolbarService]
})
export class RichTextComponent {
  @ViewChild('tributeRTE', { static: true }) rteObj: RichTextEditorComponent;

  constructor(public http: HttpClient, @Inject('BASE_URL') public baseUrl: string) {

  }

  onCreate(): void {
    this.loadTags();
  }

  loadTags(): void {
    this.http.get<Tag[]>(this.baseUrl + 'api/tag').subscribe(result => {
      const tribute: Tribute<Tag> = new Tribute({
        values: result,
        noMatchTemplate: () => '<span style:"visibility: hidden;">Não Encontrado</span>',
      });
      tribute.attach(this.rteObj.inputElement);
    }, error => console.error(error));

  }
}

interface Tag {
  key: string;
  value: string;
}
