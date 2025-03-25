import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { TokenResponse } from '../core/models/token-response';
import { TokenSearch } from '../core/models/token-search';
import { TokenSettings } from '../core/models/token-settings';
import { TokenRepository } from '../core/repositories/token.repository';

interface Form {
  description: FormControl<string | null>
  expiration: FormControl<string | null>
}

@Component({
  templateUrl: './token-registration.component.html',
  styleUrls: ['./token-registration.component.scss']
})
export class TokenRegistrationComponent implements OnInit {

  protected tokens: TokenSearch[] = []
  protected currentToken: string = '';
  protected form: FormGroup<Form> = new FormGroup({
    description: new FormControl<string | null>('', [Validators.required, Validators.minLength(3), Validators.maxLength(100)]),
    expiration: new FormControl<string | null>('', [Validators.required])
  });

  constructor(
    private readonly tokenRepository: TokenRepository
  ) { }

  ngOnInit(): void {
    this.findTokens();
  }

  protected revoke(token: TokenSearch): void {
    this.tokenRepository.revoke(token.id)
      .subscribe({
        next: () => {
          alert('Token revogado com sucesso');
          this.findTokens();
        },
        error: (e: HttpErrorResponse) => {
          alert(e.message);
        }
      })
  }

  protected save(): void {
    console.log(this.form);
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    const token: TokenSettings = this.readForm();
    this.tokenRepository.create(token)
      .subscribe({
        next: (token: TokenResponse) => {
          alert('Token cadastrado com sucesso');
          this.form.reset();
          this.findTokens();
          this.currentToken = token.token;
        },
        error: (e: HttpErrorResponse) => {
          alert(e.message);
        }
      })
  }

  protected formControlHasError(formControlName: string): boolean {
    const control = this.form.get(formControlName);
    return !control || (control.touched && !!control.errors)
  }

  protected getFormControlError(formControlName: string): string {
    const control = this.form.get(formControlName);
    if (!this.formControlHasError(formControlName) || !control) return '';
    return FormErrorsTranslate.getErrorMessage(control.errors, formControlName);
  }

  private readForm(): TokenSettings {
    const form = this.form.getRawValue();
    return {
      description: form.description ?? '',
      expiration: form.expiration ?? ''
    }
  }

  private findTokens(): void {
    this.tokenRepository.findAll()
      .subscribe({
        next: (tokens) => {
          this.tokens = tokens;
        },
        error: (e: HttpErrorResponse) => {
          alert(e.message);
        }
      });
  }

}

class FormErrorsTranslate {
  
  private static readonly errorMessages: { [key: string]: string } = {
    required: 'Este campo é obrigatório.',
    minlength: 'O valor informado é muito curto.',
    maxlength: 'O valor informado é muito longo.',
    email: 'Informe um e-mail válido.',
    pattern: 'O formato do campo está incorreto.',
    min: 'O valor é menor do que o permitido.',
    max: 'O valor é maior do que o permitido.'
  };

  public static getErrorMessage(errors: ValidationErrors | null, fieldName: string = ''): string {
    if (!errors) return '';

    for (const errorKey of Object.keys(errors)) {
      if (this.errorMessages[errorKey]) {
        return this.errorMessages[errorKey];
      }
    }

    return 'Erro desconhecido no campo ' + fieldName;
  }
}