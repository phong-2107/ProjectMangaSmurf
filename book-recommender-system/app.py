from flask import Flask,render_template,request, jsonify
import pickle
import numpy as np
# from flask_cors import CORS

app = Flask(__name__)

# load dữ liệu
popular_df = pickle.load(open('popular.pkl','rb'))
pt = pickle.load(open('pt.pkl','rb'))
books = pickle.load(open('books.pkl','rb'))
similarity_scores = pickle.load(open('similarity_scores.pkl','rb'))

@app.route('/')
def index():
    return render_template('index.html',
                           book_name = list(popular_df['Book-Id'].values),
                           author=list(popular_df['Book-Author'].values),
                           votes=list(popular_df['num_ratings'].values),
                           rating=list(popular_df['avg_rating'].values)
                           )

@app.route('/recommend')
def recommend_ui():
    return render_template('recommend.html')

@app.route('/recommend_books',methods=['post'])
def recommend():
    user_input = request.form.get('user_input')
    index = np.where(pt.index == user_input)[0][0]
    similar_items = sorted(list(enumerate(similarity_scores[index])), key=lambda x: x[1], reverse=True)[1:5]

    data = []
    for i in similar_items:
        item = []
        temp_df = books[books['Book-Title'] == pt.index[i[0]]]
        item.extend(list(temp_df.drop_duplicates('Book-Title')['Book-Title'].values))
        item.extend(list(temp_df.drop_duplicates('Book-Title')['Book-Author'].values))
        item.extend(list(temp_df.drop_duplicates('Book-Title')['Image-URL-M'].values))

        data.append(item)

    print(data)

    return render_template('recommend.html',data=data)

# API goi y sach
@app.route('/goi-y-sach', methods=['POST'])
def goi_y_sach():
    print("Request received")
    if request.is_json:
        data = request.get_json()
        print(f"Data Received: {data}")
        book_id = data.get('Book-Id')
        if book_id:
            try:
                print(f"Book ID: {book_id}")
                recommended_books = recommend(int(book_id))
                print(f"Recommended Books: {recommended_books}")
                if recommended_books:
                    recommended_books_json = [int(item[0]) for item in recommended_books]
                    return jsonify({
                        "status": 200,
                        "recommended_books_id": recommended_books_json
                    }), 200
                else:
                    return jsonify({
                        "status": 400,
                        "message": "Book ID not found"
                    }), 400
            except Exception as e:
                print(f"Error: {str(e)}")
                return jsonify({
                    "status": 400,
                    "message": f"An error occurred: {str(e)}"
                }), 400
        else:
            return jsonify({
                "status": 400,
                "message": "'Book-Id' key not found in request"
            }), 400
    else:
        return jsonify({
            "status": 400,
            "message": "Request must be in JSON format"
        }), 400
def recommend(book_id):
    book_id = int(book_id)
    # Kiểm tra xem book_name có tồn tại trong pt.index không
    if book_id in pt.index:
        # index fetch
        index = np.where(pt.index == book_id)[0][0]
        similar_items = sorted(list(enumerate(similarity_scores[index])), key=lambda x: x[1], reverse=True)[1:13]
        
        data = []
        for i in similar_items:
            item = []
            temp_df = books[books['Book-Id'] == pt.index[i[0]]]
            item.extend(list(temp_df.drop_duplicates('Book-Title')['Book-Id'].values))
            
            data.append(item)
        
        return data
    else:
        print("Book not found in database")
        return None
if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)