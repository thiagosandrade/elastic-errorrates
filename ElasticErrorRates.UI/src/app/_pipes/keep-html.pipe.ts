import { Pipe, PipeTransform, SecurityContext } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';

@Pipe(
  { 
    name: 'keepHtml', 
    pure: false,
    standalone: true
  }
)
export class KeepHtmlPipe implements PipeTransform {
  constructor(private sanitizer: DomSanitizer) {
  }

  transform(content : any) {
    console.log(content)
    return this.sanitizer.bypassSecurityTrustHtml(content);
  }
}