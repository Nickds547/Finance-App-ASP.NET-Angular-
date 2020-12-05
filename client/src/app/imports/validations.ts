import { FormControl } from '@angular/forms';

export function inParams(control: FormControl){
    let number:number =  control.value;

    console.log('in validation')
    if(number > 0)
    {
        console.log('number' + number + " is valid");
        return null
    }
        

    return number;
}
