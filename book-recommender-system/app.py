from flask import Flask,render_template,request, jsonify
import pickle
import numpy as np
from flask_cors import CORS

app = Flask(__name__)

# load dữ liệu
popular_df = pickle.load(open('popular.pkl','rb'))
pt = pickle.load(open('pt.pkl','rb'))
books = pickle.load(open('books.pkl','rb'))
similarity_scores = pickle.load(open('similarity_scores.pkl','rb'))

@app.route('/api/recommend_books', methods=['POST'])
def recommends():
    user_input = request.form.get('user_input')
    indices = np.where(pt.index == user_input)[0]

    if indices.size > 0:
        index = indices[0]
        similar_items = sorted(list(enumerate(similarity_scores[index])), key=lambda x: x[1], reverse=True)[1:5]

        data = []
        for i in similar_items:
            book_id = pt.index[i[0]]
            book_data = books[books['Book-Id'] == book_id].iloc[0]
            data.append({
                'title': book_data['Book-Title'],
                'author': book_data['Book-Author'],
                'image_url': book_data.get('Image-URL-M', 'default.jpg'),
                'votes': book_data['num_ratings'],
                'rating': book_data['avg_rating']
            })
    else:
        data = {'error': 'No matching entries found'}

    return jsonify(data)


@app.route('/')
def index():
    return render_template('index.html',
                           book_name = list(popular_df['Book-Id'].values),
                           author=list(popular_df['Book-Author'].values),
                        #    image=list(popular_df['Image-URL-M'].values),
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

        data.append(item)

    print(data)

    return render_template('recommend.html',data=data)

    CORS(app)
if __name__ == '__main__':
    app.run(debug=True, port=5001)
    
