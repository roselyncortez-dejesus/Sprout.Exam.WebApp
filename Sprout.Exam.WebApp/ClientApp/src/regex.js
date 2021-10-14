 export const validFullName = new RegExp(/^[A-Za-z,.ñÑ ]+$/);
 export const validWorkAbsentDays = new RegExp(/^(\d{1,2}|\d{0,2}\.\d{1,2})$/);
 //^(?!\.,')(?!.*\.,'$)(?!.*?\.\.)(?!.*?\ \ )(?!.*?\,\ )(?!.*?\ \,)[a-zA-Z ']{2,}(?:,[a-zA-Z. ']+)?(?:,[a-zA-Z ']+)?(?:,[a-zA-Z]+)?$