import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ApiUserService } from '../../_shared/api/user/api-user.service';
import { Message } from 'primeng/api';


@Component({
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
    registerForm: FormGroup;
    loading = false;
    submitted = false;

    public msgs: Message[] = [];

    constructor(
        private formBuilder: FormBuilder,
        private router: Router,
        private userService: ApiUserService) { }

    ngOnInit() {
        this.registerForm = this.formBuilder.group({
            firstName: ['', Validators.required],
            lastName: ['', Validators.required],
            username: ['', Validators.required],
            password: ['', [Validators.required, Validators.minLength(6)]]
        });
    }

    // convenience getter for easy access to form fields
    get f() { return this.registerForm.controls; }

    onSubmit() {
        this.submitted = true;

        // stop here if form is invalid
        if (this.registerForm.invalid) {
            return;
        }

        this.loading = true;
        this.userService.register(this.registerForm.value)
            .then(
                async (data) => {
                    this.loginStatus('Registration successful');
                    this.router.navigate(['/login']);
                },
                error => {
                    this.loginStatus(error);
                    this.loading = false;
                });
    }

    loginStatus(message : any){
        this.msgs.push({ 
            life: 3000, 
            sticky: false,
            severity: "warning", 
            summary: message,
            closable: true
          })

    }
}