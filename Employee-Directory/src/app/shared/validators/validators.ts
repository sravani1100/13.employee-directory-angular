import {
    AbstractControl,
    ValidationErrors,
    ValidatorFn
  } from '@angular/forms';
  
  export function noConsecutiveSpacesValidator(): ValidatorFn {
    return (
      control: AbstractControl
    ): ValidationErrors | null => {
  
      const value = control.value;
  
      if (!value) {
        return null;
      }
  
      return value.includes('  ')
        ? { consecutiveSpaces: true }
        : null;
    };
  }
  
  export function startWithValidator(): ValidatorFn {
    return (
      control: AbstractControl
    ): ValidationErrors | null => {
  
      const value = control.value;
  
      if (!value) {
        return null;
      }
  
      return /^[6-9]/.test(value)
        ? null
        : { invalidStart: true };
    };
  }
  
  export function mobileLengthValidator(): ValidatorFn {
    return (
      control: AbstractControl
    ): ValidationErrors | null => {
  
      const value = control.value;
  
      if (!value) {
        return null;
      }
  
      return /^\d{10}$/.test(value)
        ? null
        : { invalidLength: true };
    };
  }
  
  export function dateValidator(): ValidatorFn {
    return (
      group: AbstractControl
    ): ValidationErrors | null => {
  
      const dob = group.get('dateOfBirth')?.value;
      const joining = group.get('joiningDate')?.value;
  
      if (!dob || !joining) {
        return null;
      }
  
      const dobDate = new Date(dob);
      const joiningDate = new Date(joining);
  
      const joiningControl = group.get('joiningDate');
  
      if (!joiningControl) {
        return null;
      }
  
      if (joiningDate <= dobDate) {
  
        joiningControl.setErrors({
          ...(joiningControl.errors ?? {}),
          invalidJoiningDate: true
        });
  
        return {
          invalidJoiningDate: true
        };
      }
  
      const errors = joiningControl.errors;
  
      if (errors?.['invalidJoiningDate']) {
  
        const updatedErrors = {
          ...errors
        };
  
        delete updatedErrors['invalidJoiningDate'];
  
        joiningControl.setErrors(
          Object.keys(updatedErrors).length > 0
            ? updatedErrors
            : null
        );
      }
  
      return null;
    };
  }